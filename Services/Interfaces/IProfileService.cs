﻿using Services.DataTypeObjects;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProfileService : IDisposable
    {
        ProfileDTO GetProfile(int userId);

        void Save(ProfileDTO profile);
    }
}
