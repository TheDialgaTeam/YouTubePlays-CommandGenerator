﻿namespace TheDialgaTeam.Core.DependencyInjection
{
    public interface IServiceExecutor
    {
        void ExecuteService(ITaskCreator taskCreator);
    }
}