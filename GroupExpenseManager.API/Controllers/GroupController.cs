using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GroupExpenseManager.API.Dtos;
using GroupExpenseManager.API.Managers;
using GroupExpenseManager.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroupExpenseManager.API.Controllers
{
    [Authorize]
    [Route("api/{userId}/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> logger;
        private readonly IGroupManager groupManager;
        private readonly IMapper autoMapper;
        public GroupController(IGroupManager groupManager, ILogger<GroupController> logger, IMapper autoMapper)
        {
            this.autoMapper = autoMapper;
            this.groupManager = groupManager;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpenseSheet(GroupForCreationDto groupForCreation, int userId)
        {
            logger.LogDebug($"Creating expense sheet for userId: {userId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var newGroup = await groupManager.CreateNewGroup(autoMapper.Map<Group>(groupForCreation), userId);

            if (newGroup != null)
            {
                return CreatedAtRoute("GetGroup", new { controller = "Group", groupId = newGroup.Id, userId = userId }, autoMapper.Map<GroupToReturnDto>(newGroup));
            }

            throw new Exception("Expense sheet creation failed");
        }

        [HttpGet("{groupId}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroupDetails(int groupId, int userId)
        {
            logger.LogDebug($"Get expense sheet for userId: {userId}, groupId: {groupId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var group = await groupManager.GetGroup(groupId, userId);

            if (group != null)
            {
                return Ok(autoMapper.Map<GroupToReturnDto>(group));
            }

            return BadRequest("Group not found");
        }

        [HttpGet("{groupId}/users")]
        public async Task<IActionResult> GetGroupUsers(int groupId, int userId)
        {
            logger.LogDebug($"Get users list for userId: {userId}, groupId: {groupId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var groupUsers = await groupManager.GetGroupUsers(groupId, userId);

            if (groupUsers != null)
            {
                return Ok(autoMapper.Map<IEnumerable<UserToReturnDto>>(groupUsers));
            }

            return BadRequest("Group users not found");
        }

        [HttpGet("{groupId}/accounts")]
        public async Task<IActionResult> GetGroupAccounts(int groupId, int userId)
        {
            logger.LogDebug($"Get accounts list for userId: {userId}, groupId: {groupId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var groupAccounts = await groupManager.GetGroupAccounts(groupId, userId);

            if (groupAccounts != null)
            {
                return Ok(autoMapper.Map<IEnumerable<AccountForCreationDto>>(groupAccounts));
            }

            return BadRequest("Group not found");
        }

        [HttpGet("{groupId}/accounts/{accountId}",Name ="GetAccount")]
        public async Task<IActionResult> GetGroupAccount(int groupId, int userId, int accountId)
        {
            logger.LogDebug($"Get account details for userId: {userId}, groupId: {groupId} and account id {accountId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var account = await groupManager.GetGroupAccount(groupId, userId, accountId);

            if (account != null)
            {
                return Ok(autoMapper.Map<AccountForCreationDto>(account));
            }

            return BadRequest("Account not found");
        }

        [HttpPost("{groupId}/accounts")]
        public async Task<IActionResult> CreateNewAccount([FromBody]AccountForCreationDto accountForCreationDto,
        int groupId, int userId)
        {
            logger.LogDebug($"Create new account for userId: {userId}, groupId: {groupId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var newAccount = await groupManager.CreateNewAccount(autoMapper.Map<Account>(accountForCreationDto),
             groupId, userId);

            if (newAccount != null)
            {
                return CreatedAtRoute("GetAccount", 
                new { controller = "Group",userId = userId, groupId = groupId, accountId =newAccount.Id }, 
                autoMapper.Map<AccountForCreationDto>(newAccount));
            }

            return BadRequest("Account Creation failed");
        }

        [HttpGet("{groupId}/categories")]
        public async Task<IActionResult> GetGroupCategores(int groupId, int userId)
        {
            logger.LogDebug($"Get categories list for userId: {userId}, groupId: {groupId}");

            if (!isUserValid(userId))
            {
                return Unauthorized();
            }

            var groupCategories = await groupManager.GetGroupCategories(groupId, userId);

            if (groupCategories != null)
            {
                return Ok(groupCategories);
            }

            return BadRequest("Group not found");
        }

        private bool isUserValid(int userId)
        {
            return userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}