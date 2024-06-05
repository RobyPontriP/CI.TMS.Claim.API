using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.AspNetCore.Authentication;
using myTree.MicroService.Helper;
using Newtonsoft.Json;
using Serilog;
using APIResponse = CI.TMS.Claim.API.DTOs.Response.SharePointResponseDTO;

namespace CI.TMS.Claim.API.Helper
{
    public class SharePointHelper
    {
        private string EndPoint;
        private string SiteUrl;
        private IConfiguration _config;
        private string SystemName;
        private string ModuleName;
        public SharePointHelper(IConfiguration configuration)
        {
            _config = configuration;
            EndPoint = _config["SharePointAPIEndpoint"].ToString();
            SiteUrl = _config["SiteUrl"].ToString();
        }

        public async Task<SharePointResponseDTO.UploadResponse> uploadFileAsync(MultipartFormDataContent multiPartStream, string systemName, string moduleName, string moduleId, string AccessToken)
        {
            multiPartStream.Add(new StringContent(systemName), "system_name");
            multiPartStream.Add(new StringContent(moduleName), "module_name");
            multiPartStream.Add(new StringContent(moduleId), "module_id");
            multiPartStream.Add(new StringContent(SiteUrl), "SiteUrl");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
                    var response = await client.PostAsync($"{EndPoint}/SharePointOnline/file/Upload", multiPartStream);
                    if (response.IsSuccessStatusCode)
                    {
                        var message = response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<SharePointResponseDTO.UploadResponse>(message.Result.ToString());
                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<SharePointResponseDTO.ErrorResponse>(message.Result.ToString());

                        Log.Error($"Error SharePoint upload: {res.message}");
                        throw new Exception(res.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error SharePoint upload: {ex.Detail()}");
                throw ex;
            }

        }

        public async Task<APIResponse.GetFileUrlsResponse> GetFileUrlsAsync(string systemName, string moduleName, string moduleId, string AccessToken, string SiteUrl)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
                    var response = await client.GetAsync($"{EndPoint}/SharePointOnline/file/GetFileUrls?systemName={systemName}&moduleName={moduleName}&moduleId={moduleId}&siteurl={SiteUrl}");
                    if (response.IsSuccessStatusCode)
                    {
                        var message = response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<APIResponse.GetFileUrlsResponse>(message.Result.ToString());
                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<APIResponse.ErrorResponse>(message.Result.ToString());

                        Log.Error($"Error SharePoint upload: {res.message}");
                        throw new Exception(res.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error SharePoint upload: {ex.Detail()}");
                throw ex;
            }
        }

        public async Task<APIResponse.AssignPermissionResponse> AssignPermissionAsync(string systemName, string moduleName, string moduleId, string AccessToken, string userlist)
        {
            var value = new Dictionary<string, string>
            {
                { "systemName", systemName },
                { "moduleName", moduleName },
                { "moduleId", moduleId },
                { "userlist", userlist },
                { "SiteUrl", SiteUrl }
            };
            var content = new FormUrlEncodedContent(value);

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
                    var response = await client.PostAsync($"{EndPoint}/SharePointOnline/file/AssignPermission", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var message = response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<APIResponse.AssignPermissionResponse>(message.Result.ToString());
                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<APIResponse.ErrorResponse>(message.Result.ToString());

                        Log.Error($"Error SharePoint upload: {res.message}");
                        throw new Exception(res.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error SharePoint upload: {ex.Detail()}");
                throw ex;
            }
        }

        public async Task<APIResponse.CopyFolderResponse> CopyFolderAsync(string origin, string destination, string AccessToken)
        {
            var value = new Dictionary<string, string>
            {
                { "origin", origin },
                { "destination", destination },
                { "SiteUrl", SiteUrl }
            };
            var content = new FormUrlEncodedContent(value);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
                    var response = await client.PostAsync($"{EndPoint}/SharePointOnline/file/CopyFolder", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var message = response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<APIResponse.CopyFolderResponse>(message.Result.ToString());
                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<APIResponse.ErrorResponse>(message.Result.ToString());

                        Log.Error($"Error SharePoint upload: {res.message}");
                        throw new Exception(res.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error SharePoint upload: {ex.Detail()}");
                throw ex;
            }
        }

        public async Task<APIResponse.CopyFolderResponse> CopySpecificFilesAsync(string origin, string destination, string files, string AccessToken)
        {
            var value = new Dictionary<string, string>
            {
                { "origin", origin },
                { "destination", destination },
                { "files", files },
                { "SiteUrl", SiteUrl }
            };
            var content = new FormUrlEncodedContent(value);
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);
                    var response = await client.PostAsync($"{EndPoint}/SharePointOnline/file/CopyFolder", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var message = response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<APIResponse.CopyFolderResponse>(message.Result.ToString());
                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<APIResponse.ErrorResponse>(message.Result.ToString());

                        Log.Error($"Error SharePoint upload: {res.message}");
                        throw new Exception(res.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error SharePoint upload: {ex.Detail()}");
                throw ex;
            }
        }

    }
}
