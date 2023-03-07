using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using En = Password.Entities;

namespace Password.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class BackupController : ControllerBase
{
    IBackupService _backupService;
    public BackupController(IBackupService backupService)
    {
        _backupService = backupService;
    }
    
    [HttpGet]
    public async Task<List<En.Password>> Json()
    {
        var passwords = await _backupService.ExportPasswords(User.GetUserId());
        return passwords;
    }
}