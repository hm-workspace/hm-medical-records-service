using Microsoft.AspNetCore.Mvc;
using MedicalRecordsService.Utils.Common;
using MedicalRecordsService.InternalModels.DTOs;
using MedicalRecordsService.InternalModels.Entities;
using MedicalRecordsService.Services;

namespace MedicalRecordsService.Api.Controllers;

[ApiController]
[Route("api/medical-records")]
public class MedicalRecordsController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<PagedResult<MedicalRecordDto>>> GetAll([FromQuery] SearchQuery search)
    {
        var query = MedicalRecordsStore.Records.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(search.SearchTerm))
        {
            query = query.Where(x =>
                x.RecordNumber.Contains(search.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                x.Diagnosis.Contains(search.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                x.TreatmentPlan.Contains(search.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var total = query.Count();
        var items = query
            .OrderByDescending(x => x.RecordDate)
            .Skip((search.PageNumber - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(MedicalRecordDto.FromEntity)
            .ToList();
        return Ok(ApiResponse<PagedResult<MedicalRecordDto>>.Ok(new PagedResult<MedicalRecordDto>(items, total, search.PageNumber, search.PageSize)));
    }

    [HttpGet("{id:int}")]
    public ActionResult<ApiResponse<MedicalRecordDto>> GetById(int id)
    {
        var record = MedicalRecordsStore.Records.FirstOrDefault(x => x.Id == id);
        if (record is null)
        {
            return NotFound(ApiResponse<MedicalRecordDto>.Fail("Medical record not found"));
        }

        return Ok(ApiResponse<MedicalRecordDto>.Ok(MedicalRecordDto.FromEntity(record)));
    }

    [HttpGet("patient/{patientId:int}")]
    public ActionResult<ApiResponse<IEnumerable<MedicalRecordDto>>> GetByPatientId(int patientId)
    {
        var records = MedicalRecordsStore.Records
            .Where(x => x.PatientId == patientId)
            .OrderByDescending(x => x.RecordDate)
            .Select(MedicalRecordDto.FromEntity)
            .ToList();
        return Ok(ApiResponse<IEnumerable<MedicalRecordDto>>.Ok(records));
    }

    [HttpPost]
    public ActionResult<ApiResponse<MedicalRecordDto>> Create([FromBody] CreateMedicalRecordDto dto)
    {
        var id = Interlocked.Increment(ref MedicalRecordsStore.RecordSeed);
        var record = new MedicalRecordEntity
        {
            Id = id,
            RecordNumber = dto.RecordNumber,
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            RecordDate = dto.RecordDate,
            Diagnosis = dto.Diagnosis,
            TreatmentPlan = dto.TreatmentPlan,
            Notes = dto.Notes
        };

        MedicalRecordsStore.Records.Add(record);
        return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<MedicalRecordDto>.Ok(MedicalRecordDto.FromEntity(record), "Medical record created"));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ApiResponse<MedicalRecordDto>> Update(int id, [FromBody] UpdateMedicalRecordDto dto)
    {
        var record = MedicalRecordsStore.Records.FirstOrDefault(x => x.Id == id);
        if (record is null)
        {
            return NotFound(ApiResponse<MedicalRecordDto>.Fail("Medical record not found"));
        }

        record.DoctorId = dto.DoctorId;
        record.RecordDate = dto.RecordDate;
        record.Diagnosis = dto.Diagnosis;
        record.TreatmentPlan = dto.TreatmentPlan;
        record.Notes = dto.Notes;
        return Ok(ApiResponse<MedicalRecordDto>.Ok(MedicalRecordDto.FromEntity(record), "Medical record updated"));
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ApiResponse<string>> Delete(int id)
    {
        var record = MedicalRecordsStore.Records.FirstOrDefault(x => x.Id == id);
        if (record is null)
        {
            return NotFound(ApiResponse<string>.Fail("Medical record not found"));
        }

        MedicalRecordsStore.Records.Remove(record);
        return Ok(ApiResponse<string>.Ok("Medical record deleted"));
    }
}


