using Application.Logic;
using Application.LogicInterfaces;
using Data.DAOs;
using Domain.DTOs.Meeting;
using Grpc.Core;


namespace Unit_Tests;

[TestClass]
public class MeetingUnitTests
{
    [TestMethod]
    public void InvalidDateTest()
    {
        try
        {
            //Initializing Logic class
            IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
            //Initializing new object with incorrect dates
            MeetingCreateDTO meeting = new MeetingCreateDTO()
            {
                AssignedLeader = 192,
                ProjectAssigned = 1,
                Title = "Test title",
                Description = "Test description",
                StartDate = new DateTime(0, 0, 0, 0, 0, 0),
                EndDate = new DateTime(0, 0, 0, 0, 0, 0)
            };
            
            //performing action on the logic
            logic.CreateAsync(meeting);
        }
        catch (ArgumentOutOfRangeException e)
        {
            //Assert
            StringAssert.Contains(e.Message, "parameters describe an un-representable DateTime");
        }
    }

    [TestMethod]
    public void LeaderNotFoundTest()
    {
        //Initializing Logic class
        IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
        //Initializing new object with incorrect dates
        MeetingCreateDTO meeting = new MeetingCreateDTO()
        {
            AssignedLeader = 0,
            ProjectAssigned = 293,
            Title = "Test title",
            Description = "Test description",
            StartDate = new DateTime(1, 1, 1, 0, 0, 0),
            EndDate = new DateTime(1, 1, 1, 1, 0, 0)
        };
        
        //Assert
        var expectedErrorMessage = $"Member not found";
        Task<RpcException> exception = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(meeting));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Status.Detail);
    }
    
    [TestMethod]
    public void ProjectNotFoundTest()
    {
        //Initializing Logic class
        IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
        //Initializing new object with incorrect dates
        MeetingCreateDTO meeting = new MeetingCreateDTO()
        {
            AssignedLeader = 192,
            ProjectAssigned = 0,
            Title = "Test title",
            Description = "Test description",
            StartDate = new DateTime(1, 1, 1, 0, 0, 0),
            EndDate = new DateTime(1, 1, 1, 1, 0, 0)
        };
        
        //Assert
        var expectedErrorMessage = "Project not found";
        Task<RpcException> exception = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(meeting));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Status.Detail);
    }
    
    [TestMethod]
    public void EmptyTitleTest()
    {
        //Initializing Logic class
        IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
        //Initializing new object with incorrect dates
        MeetingCreateDTO meeting = new MeetingCreateDTO()
        {
            AssignedLeader = 192,
            ProjectAssigned = 293,
            Title = "",
            Description = "Test description",
            StartDate = new DateTime(1, 1, 1, 0, 0, 0),
            EndDate = new DateTime(1, 1, 1, 1, 0, 0)
        };
        
        //Assert
        var expectedErrorMessage = "Title must not be empty.";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(meeting));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    } 
    
    [TestMethod]
    public void OutOfBoundsTitleTest()
    {
        //Initializing Logic class
        IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
        //Initializing new object with incorrect dates
        MeetingCreateDTO meeting = new MeetingCreateDTO()
        {
            AssignedLeader = 192,
            ProjectAssigned = 293,
            Title = "bruh bruh bruh bruh bruh bruh",
            Description = "Test description",
            StartDate = new DateTime(1, 1, 1, 0, 0, 0),
            EndDate = new DateTime(1, 1, 1, 1, 0, 0)
        };
        
        //Assert
        var expectedErrorMessage = "Title must be between 3 and 20 characters.";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(meeting));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    }
    
    [TestMethod]
    public void OutOfBoundsDescriptionTest()
    {
        //Initializing Logic class
        IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
        //Initializing new object with incorrect dates
        MeetingCreateDTO meeting = new MeetingCreateDTO()
        {
            AssignedLeader = 192,
            ProjectAssigned = 293,
            Title = "Test title",
            Description = " bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh",
            StartDate = new DateTime(1, 1, 1, 0, 0, 0),
            EndDate = new DateTime(1, 1, 1, 1, 0, 0)
        };
        
        //Assert
        var expectedErrorMessage = "Description cannot be longer than 250 characters.";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(meeting));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    }

    [TestMethod]
    public void CreatingMeetingTest()
    {
        //Initializing Logic class
        IMeetingLogic logic = new MeetingLogic(new MeetingDAO(), new MemberDAO(), new ProjectDAO());
        
        //Initializing new object with incorrect dates
        MeetingCreateDTO meeting = new MeetingCreateDTO()
        {
            AssignedLeader = 192,
            ProjectAssigned = 293,
            Title = "Test title",
            Description = "Test description",
            StartDate = new DateTime(1, 1, 1, 0, 0, 0),
            EndDate = new DateTime(1, 1, 1, 1, 0, 0)
        };
    }
    
    
}