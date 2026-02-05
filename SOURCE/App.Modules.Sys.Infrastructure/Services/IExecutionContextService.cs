using System;
using System.Collections.Generic;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Infrastructure.Services;

/// <summary>
/// Base execution context abstraction.
/// Provides runtime execution information independent of transport mechanism (HTTP, gRPC, Message Queue, etc.).
/// </summary>
/// <remarks>
/// Lifetime: Scoped (per execution unit - request, message, job).
/// 
/// This is the BASE abstraction - specific transports extend this:
/// - IRequestContextService (HTTP-specific) : IExecutionContextService
/// - IMessageContextService (Queue-specific) : IExecutionContextService
/// - IJobContextService (Background job-specific) : IExecutionContextService
/// 
/// Use cases:
/// - Logging (correlation ID, timing)
/// - Diagnostics (execution tracking)
/// - Security (execution identity)
/// - Tracing (distributed tracing)
/// </remarks>
public interface IExecutionContextService : IHasService
{
    // ========================================
    // EXECUTION IDENTIFICATION
    // ========================================

    /// <summary>
    /// Unique execution identifier (for correlation in logs/traces).
    /// Generated per execution unit (request, message, job).
    /// </summary>
    string ExecutionId { get; }

    /// <summary>
    /// Execution type (Request, Message, Job, etc.).
    /// Identifies what kind of execution this is.
    /// </summary>
    ExecutionType ExecutionType { get; }

    /// <summary>
    /// When execution started (UTC).
    /// </summary>
    DateTime ExecutionStartTimeUtc { get; }

    /// <summary>
    /// Duration since execution start.
    /// Useful for performance monitoring.
    /// </summary>
    TimeSpan ExecutionDuration { get; }

    // ========================================
    // EXECUTION METADATA
    // ========================================

    /// <summary>
    /// Arbitrary metadata associated with this execution.
    /// Can store custom values for correlation, tracing, etc.
    /// </summary>
    IReadOnlyDictionary<string, object?> Metadata { get; }

    /// <summary>
    /// Get specific metadata value by key.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <returns>Metadata value if exists, null otherwise.</returns>
    object? GetMetadata(string key);

    /// <summary>
    /// Set metadata value (if mutable context).
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    void SetMetadata(string key, object? value);

    // ========================================
    // CORRELATION & TRACING
    // ========================================

    /// <summary>
    /// Parent execution ID (for distributed tracing).
    /// If this execution was triggered by another execution, this contains the parent's ID.
    /// </summary>
    string? ParentExecutionId { get; }

    /// <summary>
    /// Trace ID (for distributed tracing across services).
    /// All related executions share the same trace ID.
    /// </summary>
    string? TraceId { get; }

    /// <summary>
    /// Span ID (for distributed tracing).
    /// Unique identifier for this span within the trace.
    /// </summary>
    string? SpanId { get; }
}

/// <summary>
/// Execution type enumeration.
/// Identifies the kind of execution context.
/// </summary>
public enum ExecutionType
{
    /// <summary>
    /// Unknown execution type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// HTTP request execution (REST API, Web UI).
    /// </summary>
    Request = 1,

    /// <summary>
    /// Message queue execution (RabbitMQ, Azure Service Bus, etc.).
    /// </summary>
    Message = 2,

    /// <summary>
    /// Background job execution (Hangfire, Quartz, etc.).
    /// </summary>
    Job = 3,

    /// <summary>
    /// Scheduled task execution (CRON, Timer).
    /// </summary>
    ScheduledTask = 4,

    /// <summary>
    /// gRPC call execution.
    /// </summary>
    GrpcCall = 5,

    /// <summary>
    /// SignalR/WebSocket execution.
    /// </summary>
    WebSocket = 6,

    /// <summary>
    /// Command-line execution (CLI tool, console app).
    /// </summary>
    CommandLine = 7
}
