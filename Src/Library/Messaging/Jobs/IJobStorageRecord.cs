﻿// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace FastEndpoints;

/// <summary>
/// the contract for a job storage record entity
/// </summary>
public interface IJobStorageRecord
{
    /// <summary>
    /// a unique id for the job queue. each command type has its own queue. this is automatically generated by the library.
    /// </summary>
    string QueueID { get; set; }

    /// <summary>
    /// a unique id used to track a particular job for the purpose of progress monitoring and/or termination.
    /// </summary>
    Guid TrackingID { get; set; }

    /// <summary>
    /// the actual command object that will be embedded in the storage record.
    /// if your database/orm (such as ef-core) doesn't support embedding objects, you can take the following steps:
    /// <code>
    /// 1. add a [NotMapped] attribute to this property.
    /// 2. add a new property, either a <c>string</c> or <c>byte[]</c>
    /// 3. implement both GetCommand() and SetCommand() methods to serialize/deserialize the command object back and forth and store it in the newly added property.
    /// </code>
    /// you may use any serializer you please. recommendation is to use MessagePack.
    /// </summary>
    object Command { get; set; }

    /// <summary>
    /// the job will not be executed before this date/time. by default, it will automatically be set to the time of creation allowing jobs to be executed as soon as they're
    /// created.
    /// </summary>
    DateTime ExecuteAfter { get; set; }

    /// <summary>
    /// the expiration date/time of job. if the job remains in an incomplete state past this time, the record is considered stale, and will be marked for removal from
    /// storage.
    /// </summary>
    DateTime ExpireOn { get; set; }

    /// <summary>
    /// indicates whether the job has successfully completed or not.
    /// </summary>
    bool IsComplete { get; set; }

    /// <summary>
    /// implement this function to customize command deserialization.
    /// </summary>
    TCommand GetCommand<TCommand>() where TCommand : class, ICommandBase
        => (TCommand)Command;

    /// <summary>
    /// implement this method to customize command serialization.
    /// </summary>
    void SetCommand<TCommand>(TCommand command) where TCommand : class, ICommandBase
        => Command = command;
}

/// <summary>
/// addon interface to enable storage of job results on a job storage record (<see cref="IJobStorageRecord" />)
/// </summary>
public interface IJobResultStorage
{
    /// <summary>
    /// the actual result object that will be embedded in the storage record.
    /// if your database/orm (such as ef-core) doesn't support embedding objects, you can take the following steps:
    /// <code>
    /// 1. add a [NotMapped] attribute to this property.
    /// 2. add a new property, either a <c>string</c> or <c>byte[]</c>
    /// 3. implement both GetResult() and SetResult() methods to serialize/deserialize the command object back and forth and store it in the newly added property.
    /// </code>
    /// you may use any serializer you please. recommendation is to use MessagePack.
    /// </summary>
    object? Result { get; set; }

    /// <summary>
    /// implement this function to customize the result deserialization.
    /// </summary>
    TResult? GetResult<TResult>()
        => Result is null ? default : (TResult)Result;

    /// <summary>
    /// implement this method to customize the result serialization.
    /// </summary>
    void SetResult<TResult>(TResult result)
        => Result = result;
}

/// <summary>
/// implement this interface on your job storage record if you'd like to persist the full type name of the command class which is associated with the storage record.
/// you don't need to set the value yourself as it will be automatically set by the system.
/// </summary>
interface IHasCommandType
{
    string CommandType { get; set; }
}