using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TransitDesk.Web.Filters;

public class AuditLogAttribute : ActionFilterAttribute
{
    private const string StopwatchKey = "AuditStopwatch";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        context.HttpContext.Items[StopwatchKey] = stopwatch;

        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var controller = context.RouteData.Values["controller"]?.ToString();
        var action = context.RouteData.Values["action"]?.ToString();
        var utcTimestamp = DateTime.UtcNow.ToString("o");

        Debug.WriteLine($"[AUDIT START] IP: {ip} | Kontroler: {controller} | Akcja: {action} | UTC: {utcTimestamp}");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.HttpContext.Items.TryGetValue(StopwatchKey, out var obj) && obj is Stopwatch stopwatch)
        {
            stopwatch.Stop();
            var durationMs = stopwatch.ElapsedMilliseconds.ToString();

            context.HttpContext.Response.Headers.TryAdd("X-Execution-Duration-Ms", durationMs);
            Debug.WriteLine($"[AUDIT END] Czas wykonania: {durationMs} ms");
        }
    }
}