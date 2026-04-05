using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalRecordsService.Utils.Common;
using MedicalRecordsService.InternalModels.DTOs;
using MedicalRecordsService.Services;

namespace MedicalRecordsService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/medical-records")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalRecordService _service;

    public MedicalRecordsController(IMedicalRecordService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<MedicalRecordDto>>>> GetAll([FromQuery] SearchQuery search)
    {
        var records = string.IsNullOrWhiteSpace(search.SearchTerm)
            ? await _service.GetAllAsync()
            : await _service.SearchAsync(search.SearchTerm);

        var total = records.Count;
        var items = records
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(MedicalRecordDto.FromEntity)
            .ToList();
        return Ok(ApiResponse<PagedResult<MedicalRecordDto>>.Ok(new PagedResult<MedicalRecordDto>(items, total, search.PageNumber, search.PageSize)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<MedicalRecordDto>>> GetById(int id)
    {
        var record = await _service.GetByIdAsync(id);
        if (record is null)
        {
            return NotFound(ApiResponse<MedicalRecordDto>.Fail("Medical record not found"));
        }

        return Ok(ApiResponse<MedicalRecordDto>.Ok(MedicalRecordDto.FromEntity(record)));
    }

    [HttpGet("patient/{patientId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MedicalRecordDto>>>> GetByPatientId(int patientId)
    {
        var records = await _service.GetByPatientIdAsync(patientId);
        var dtos = records.Select(MedicalRecordDto.FromEntity).ToList();
        return Ok(ApiResponse<IEnumerable<MedicalRecordDto>>.Ok(dtos));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MedicalRecordDto>>> Create([FromBody] CreateMedicalRecordDto dto)
    {
        var createdRecord = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdRecord.Id }, ApiResponse<MedicalRecordDto>.Ok(MedicalRecordDto.FromEntity(createdRecord), "Medical record created"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<MedicalRecordDto>>> Update(int id, [FromBody] UpdateMedicalRecordDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        if (!success)
        {
            return NotFound(ApiResponse<MedicalRecordDto>.Fail("Medical record not found"));
        }

        var record = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<MedicalRecordDto>.Ok(MedicalRecordDto.FromEntity(record!), "Medical record updated"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success)
        {
            return NotFound(ApiResponse<string>.Fail("Medical record not found"));
        }

        return Ok(ApiResponse<string>.Ok("Medical record deleted"));
    }
}


