﻿using Microsoft.Practices.Unity;
using Owin;

namespace IISAdmin.Interfaces
{
    public interface IBackgroundWorker {
        void Init(IAppBuilder appBuilder);
    }
}
