using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Trasys.PhotoTrack.Api.Models
{
    /// <summary>
    /// Enum that represent the status possibles when a user tries to authenticate by using Entity
    /// </summary>
    public enum AuthenticationResult
    {
        /// <summary>
        /// Authentication successful
        /// </summary>
        [DescriptionAttribute("Authentication successful")]
        Success,
        /// <summary>
        /// Credentials error, login or password
        /// </summary>
        [DescriptionAttribute("Login or password incorrect")]
        BadCredentials,
        /// <summary>
        /// The profile specified don't exists
        /// </summary>
        [DescriptionAttribute("Profile not found")]
        ProfileNotFound,
        /// <summary>
        /// Unknown error
        /// </summary>
        [DescriptionAttribute("Unknown error occurs")]
        Unknown
    }
}