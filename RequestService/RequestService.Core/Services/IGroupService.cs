using HelpMyStreet.Contracts.GroupService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Utils.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public interface IGroupService
    {
        Task<GetGroupLocationsResponse> GetGroupLocations(int groupId);
        Task<GetNewRequestActionsResponse> GetNewRequestActions(GetNewRequestActionsRequest request, CancellationToken cancellationToken);
        Task<GetUserGroupsResponse> GetUserGroups(int userId, CancellationToken cancellationToken);
        Task<GetGroupMembersResponse> GetGroupMembers(int groupID);
        Task<GetUserRolesResponse> GetUserRoles(int userId, CancellationToken cancellationToken);
        Task<PostAssignRoleResponse> PostAssignRole(PostAssignRoleRequest postAssignRoleRequest, CancellationToken cancellationToken);
        Task<GetGroupMemberDetailsResponse> GetGroupMemberDetails(GetGroupMemberDetailsRequest request);
        Task<GetGroupMemberResponse> GetGroupMember(GetGroupMemberRequest request);
        Task<GetGroupActivityCredentialsResponse> GetGroupActivityCredentials(GetGroupActivityCredentialsRequest request);
        Task<GetRequestHelpFormVariantResponse> GetRequestHelpFormVariant(int groupId, string source, CancellationToken cancellationToken);
        Task<GetNewShiftActionsResponse> GetNewShiftActions(GetNewShiftActionsRequest request, CancellationToken cancellationToken);
        Task<GetGroupResponse> GetGroup(int groupID);
        Task<GetChildGroupsResponse> GetChildGroups(int groupID);
        Task<double?> GetGroupSupportActivityRadius(int groupID, SupportActivities supportActivity);
    }

}
