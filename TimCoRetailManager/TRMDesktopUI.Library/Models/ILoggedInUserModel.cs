﻿using System;

namespace TRMDesktopUI.Library.Models
{
    public interface ILoggedInUserModel
    {
        DateTime CreatedDate { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }

        void ResetUserModel();
    }
}