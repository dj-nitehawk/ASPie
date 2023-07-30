﻿namespace TestCases.JobQueueTest;

public class JobTestCommand : ICommand
{
    public static List<int> CompletedIDs = new();

    public int Id { get; set; }

    public bool ShouldThrow { get; set; }
}

public class JobTestCommandHandler : ICommandHandler<JobTestCommand>
{
    public Task ExecuteAsync(JobTestCommand cmd, CancellationToken ct)
    {
        if (cmd.ShouldThrow)
            throw new InvalidOperationException();

        JobTestCommand.CompletedIDs.Add(cmd.Id);

        return Task.CompletedTask;
    }
}