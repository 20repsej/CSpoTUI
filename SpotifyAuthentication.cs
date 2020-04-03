using System;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

public static async void Example()
{
  SpotifyWebAPI api = new SpotifyWebAPI
  {
      AccessToken = "XX?X?X",
      TokenType = "Bearer"
  };
  
  PrivateProfile profile = await api.GetPrivateProfileAsync();
  if(!profile.HasError()) {
    Console.WriteLine(profile.DisplayName);
  }
}



