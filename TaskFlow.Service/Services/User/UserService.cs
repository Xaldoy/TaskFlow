using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Service.DTOs.Result;
using TaskFlow.DAL.Repositories.User;
using TaskFlow.Model.Models;
using TaskFlow.Service.DTOs.Message;
using TaskFlow.Service.DTOs.User;
using TaskFlow.Service.Services.Authorization;

namespace TaskFlow.Service.Services.User
{
    public class UserService(IAuthorizationService authorizationService, UserManager<AppUser> userManager, IUserRepository userRepository, IMapper mapper) : IUserService
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ServiceResult> SendFriendRequest(string username)
        {
            string userId = _authorizationService.GetUserId();

            AppUser? sendingTo = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (sendingTo == null)
            {
                return ServiceResult.Success(new FriendRequestResultDto()
                {
                    Success = false,
                    Message = "User not found!",
                });
            }

            var alreadyExists = await _userRepository.GetFriendRelationForUsers(userId, sendingTo.Id);

            if (alreadyExists != null)
            {
                return ServiceResult.Success(new FriendRequestResultDto()
                {
                    Success = false,
                    Message = alreadyExists.Accepted switch
                    {
                        true => $"You are already friends with {username}.",
                        null => $"Friend request to {username} already sent!",
                        false => $"This user does not want to be your friend!",
                    },
                });
            }

            FriendRelation friendRequest = new()
            {
                User1Id = userId,
                User2Id = sendingTo.Id,
                Accepted = null,
            };

            await _userRepository.CreateFriendRelation(friendRequest);

            return ServiceResult.Success(new FriendRequestResultDto()
            {
                Success = true,
                Message = "Friend request sent!",
            });

        }

        public async Task<ServiceResult> GetFriends()
        {
            string userId = _authorizationService.GetUserId();
            var friends = (await _userRepository.GetFriends(userId)).Select(x => new AppUserDto()
            {
                Username = x.User1Id == userId ? x.User2.UserName! : x.User1.UserName!,
            });
            return ServiceResult.Success(friends);
        }

        public async Task<ServiceResult> GetReceivedFriendRequests()
        {
            string userId = _authorizationService.GetUserId();
            var receivedFriendRequests = _mapper.Map<List<ReceivedFriendRequestDto>>(await _userRepository.GetReceivedFriendRequests(userId));
            return ServiceResult.Success(receivedFriendRequests);
        }

        public async Task<ServiceResult> GetSentFriendRequests()
        {
            string userId = _authorizationService.GetUserId();
            var sentFriendRequests = _mapper.Map<List<SentFriendRequestDto>>(await _userRepository.GetSentFriendRequests(userId));
            return ServiceResult.Success(sentFriendRequests);
        }

        public async Task<ServiceResult> DeleteFriendRequest(int friendRelationId)
        {
            if (!await _authorizationService.UserOwnsSentFriendRelation(friendRelationId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            await _userRepository.DeleteFriendRequest(friendRelationId);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> AcceptFriendRequest(int friendRelationId)
        {
            if (!await _authorizationService.UserOwnsReceivedFriendRelation(friendRelationId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            await _userRepository.AcceptFriendRequest(friendRelationId);
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> DeclineFriendRequest(int friendRelationId)
        {
            if (!await _authorizationService.UserOwnsReceivedFriendRelation(friendRelationId))
                return ServiceResult.Failure(MessageDescriber.Unauthorized());

            await _userRepository.DeclineFriendRequest(friendRelationId);
            return ServiceResult.Success();
        }
    }
}
