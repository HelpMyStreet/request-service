﻿using HelpMyStreet.Cache;
using HelpMyStreet.Contracts.GroupService.Request;
using HelpMyStreet.Contracts.GroupService.Response;
using HelpMyStreet.Contracts.RequestService.Response;
using HelpMyStreet.Contracts.Shared;
using HelpMyStreet.Utils.Enums;
using HelpMyStreet.Utils.Utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RequestService.Core.Services
{
    public class GroupService : IGroupService
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IMemDistCache<double?> _memDistCache;
        private const string CACHE_KEY_PREFIX = "group-service-";
        public GroupService(IHttpClientWrapper httpClientWrapper, IMemDistCache<double?> memDistCache)
        {
            _httpClientWrapper = httpClientWrapper;
            _memDistCache = memDistCache;
        }

        public async Task<GetGroupActivityCredentialsResponse> GetGroupActivityCredentials(GetGroupActivityCredentialsRequest request)
        {
            string path = $"/api/GetGroupActivityCredentials";
            string absolutePath = $"{path}";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            
            using (HttpResponseMessage response = await _httpClientWrapper.PostAsync(HttpClientConfigName.GroupService, absolutePath, jsonContent, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupActivityCredentialsResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetGroupMemberResponse> GetGroupMember(GetGroupMemberRequest request)
        {
            string path = $"/api/GetGroupMember?groupID={request.GroupId}&userId={request.UserId}&authorisingUserId={request.AuthorisingUserId}";
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupMemberResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetGroupMemberDetailsResponse> GetGroupMemberDetails(GetGroupMemberDetailsRequest request)
        {
            string path = $"/api/GetGroupMemberDetails?groupID={request.GroupId}&userId={request.UserId}&authorisingUserId={request.AuthorisingUserId}";
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupMemberDetailsResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetGroupMembersResponse> GetGroupMembers(int groupID)
        {
            string path = $"/api/GetGroupMembers?groupID=" + groupID;
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupMembersResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetNewRequestActionsSimplifiedResponse> GetNewRequestActionsSimplified(GetNewRequestActionsSimplifiedRequest request, CancellationToken cancellationToken)
        {
            string path = $"api/GetNewRequestActionsSimplified";

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, path, request, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetNewRequestActionsSimplifiedResponse, GroupServiceErrorCode>>(content);
                if (jsonResponse.IsSuccessful)
                {
                    return jsonResponse.Content;
                }
            }
            throw new Exception("Unable to get new request actions simplified");
        }

        public async Task<GetNewShiftActionsResponse> GetNewShiftActions(GetNewShiftActionsRequest request, CancellationToken cancellationToken)
        {
            string path = $"api/GetNewShiftActions";

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, path, request, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetNewShiftActionsResponse, GroupServiceErrorCode>>(content);
                if (jsonResponse.IsSuccessful)
                {
                    return jsonResponse.Content;
                }
            }
            throw new Exception("Unable to get new request actions");
        }

        public async Task<GetRequestHelpFormVariantResponse> GetRequestHelpFormVariant(int groupId, string source, CancellationToken cancellationToken)
        {
            string path = $"api/GetRequestHelpFormVariant?GroupID={groupId}&Source={source}";

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetRequestHelpFormVariantResponse, GroupServiceErrorCode>>(content);
                if (jsonResponse.IsSuccessful)
                {
                    return jsonResponse.Content;
                }
            }
            throw new Exception("Unable to get user groups");
        }

        public async Task<GetUserGroupsResponse> GetUserGroups(int userId, CancellationToken cancellationToken)
        {
            string path = $"api/GetUserGroups?UserID={userId}";

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetUserGroupsResponse, GroupServiceErrorCode>>(content);
                if (jsonResponse.IsSuccessful)
                {
                    return jsonResponse.Content;
                }
            }
            throw new Exception("Unable to get user groups");
        }

        public async Task<GetUserRolesResponse> GetUserRoles(int userId, CancellationToken cancellationToken)
        {
            string path = $"api/GetUserRoles?UserID={userId}";

            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, path, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetUserRolesResponse, GroupServiceErrorCode>>(content);
                if (jsonResponse.IsSuccessful)
                {
                    return jsonResponse.Content;
                }
            }
            throw new Exception("Unable to get user roles");
        }

        public async Task<PostAssignRoleResponse> PostAssignRole(PostAssignRoleRequest postAssignRoleRequest, CancellationToken cancellationToken)
        {
            string path = $"api/PostAssignRole";

            string json = JsonConvert.SerializeObject(postAssignRoleRequest);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await _httpClientWrapper.PostAsync(HttpClientConfigName.GroupService, path, data, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ResponseWrapper<PostAssignRoleResponse, GroupServiceErrorCode>>(content);
                if (jsonResponse.IsSuccessful)
                {
                    return jsonResponse.Content;
                }
            }
            throw new Exception("Unable to assign role");
        }

        public async Task<GetGroupResponse> GetGroup(int groupID)
        {
            string path = $"/api/GetGroup?groupID=" + groupID;
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetChildGroupsResponse> GetChildGroups(int groupID)
        {
            string path = $"/api/GetChildGroups?groupID=" + groupID;
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetChildGroupsResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<GetGroupLocationsResponse> GetGroupLocations(int groupId)
        {
            string path = $"/api/GetGroupLocations?GroupID={groupId}&IncludeChildGroups=true";
            string absolutePath = $"{path}";
            using (HttpResponseMessage response = await _httpClientWrapper.GetAsync(HttpClientConfigName.GroupService, absolutePath, CancellationToken.None).ConfigureAwait(false))
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var getJobsResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupLocationsResponse, GroupServiceErrorCode>>(jsonResponse);
                if (getJobsResponse.HasContent && getJobsResponse.IsSuccessful)
                {
                    return getJobsResponse.Content;
                }
                return null;
            }
        }

        public async Task<double?> GetGroupSupportActivityRadius(int groupID, SupportActivities supportActivity, CancellationToken cancellationToken)
        {
            return await _memDistCache.GetCachedDataAsync(async (cancellationToken) =>
            {
                GetGroupSupportActivityRadiusRequest request = new GetGroupSupportActivityRadiusRequest() { GroupId = groupID, SupportActivityType = new SupportActivityType() { SupportActivity = supportActivity } };
                string json = JsonConvert.SerializeObject(request);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await _httpClientWrapper.PostAsync(HttpClientConfigName.GroupService, "/api/GetGroupSupportActivityRadius", data, CancellationToken.None))
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var getRadiusResponse = JsonConvert.DeserializeObject<ResponseWrapper<GetGroupSupportActivityRadiusResponse, GroupServiceErrorCode>>(jsonResponse);
                    if (getRadiusResponse.HasContent && getRadiusResponse.IsSuccessful)
                    {
                        return getRadiusResponse.Content.SupportRadiusMiles;
                    }
                    else
                    {
                        throw new HttpRequestException("Unable to fetch radius details");
                    }
                }
            }, $"{CACHE_KEY_PREFIX}-group-{groupID}-sa-{(int)supportActivity}", RefreshBehaviour.WaitForFreshData, cancellationToken);//Ensure RefreshBehaviour is set to WaitForFreshData if no other mechanisms are implace to refresh data.  DoNotWaitForFreshData fails in an Azure Function
        }
    }
}
