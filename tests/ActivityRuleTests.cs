using System;
using U_VoluntApp_Backend.Src.Domain.Entities.Activity;
using Xunit;

namespace U_VoluntApp_Backend.Tests;

public class ActivityRuleTests
{
    [Fact]
    public void Create_WithValidInputs_CreatesRule()
    {
        var now = DateTime.UtcNow;

        var rule = ActivityRule.Create(
            activityCode: "ACT-01",
            requiresEnrollment: true,
            requiresApproval: false,
            countsVolunteerHours: true,
            photoUrl: null,
            enrollmentDeadline: now.AddDays(5),
            startDate: now.AddDays(10),
            totalCapacity: 30,
            costAmount: 0m,
            nowUtc: now);

        Assert.NotEmpty(rule.UvaCode);
        Assert.Equal("ACT-01", rule.ActivityCode);
        Assert.Equal(30, rule.TotalCapacity);
        Assert.Equal(0m, rule.CostAmount);
        Assert.NotNull(rule.PhotoUrl);
    }

    [Fact]
    public void Create_WithEmptyActivityCode_Throws()
    {
        var now = DateTime.UtcNow;

        Assert.Throws<InvalidOperationException>(() => ActivityRule.Create(
            activityCode: string.Empty,
            requiresEnrollment: false,
            requiresApproval: false,
            countsVolunteerHours: false,
            photoUrl: null,
            enrollmentDeadline: null,
            startDate: null,
            totalCapacity: 0,
            costAmount: 0m,
            nowUtc: now));
    }

    [Fact]
    public void HasCapacity_TotalCapacityZero_ReturnsTrue()
    {
        var now = DateTime.UtcNow;

        var rule = ActivityRule.Create(
            activityCode: "ACT-02",
            requiresEnrollment: false,
            requiresApproval: false,
            countsVolunteerHours: false,
            photoUrl: null,
            enrollmentDeadline: null,
            startDate: null,
            totalCapacity: 0,
            costAmount: 0m,
            nowUtc: now);

        Assert.True(rule.HasCapacity(1000));
    }

    [Fact]
    public void HasTimeForRegister_CheckBeforeDeadline_ReturnsTrue()
    {
        var now = DateTime.UtcNow;
        var enrollmentDeadline = now.AddDays(3);
        var startDate = now.AddDays(10);

        var rule = ActivityRule.Create(
            activityCode: "ACT-03",
            requiresEnrollment: true,
            requiresApproval: false,
            countsVolunteerHours: false,
            photoUrl: null,
            enrollmentDeadline: enrollmentDeadline,
            startDate: startDate,
            totalCapacity: 10,
            costAmount: 0m,
            nowUtc: now);

        Assert.True(rule.HasTimeForRegister(now));
    }

    [Fact]
    public void ApplyUpdate_NoChanges_Throws()
    {
        var now = DateTime.UtcNow;
        var enrollmentDeadline = now.AddDays(5);
        var startDate = now.AddDays(10);

        var rule = ActivityRule.Create(
            activityCode: "ACT-04",
            requiresEnrollment: true,
            requiresApproval: false,
            countsVolunteerHours: true,
            photoUrl: null,
            enrollmentDeadline: enrollmentDeadline,
            startDate: startDate,
            totalCapacity: 5,
            costAmount: 1m,
            nowUtc: now);

        Assert.Throws<InvalidOperationException>(() => rule.ApplyUpdate(
            requiresEnrollment: true,
            requiresApproval: false,
            countsVolunteerHours: true,
            photoUrl: null,
            enrollmentDeadline: enrollmentDeadline,
            startDate: startDate,
            totalCapacity: 5,
            costAmount: 1m,
            nowUtc: now));
    }
}