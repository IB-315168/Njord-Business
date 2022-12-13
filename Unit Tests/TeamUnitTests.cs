using Grpc.Core;

namespace Unit_Tests;
using Application.Logic;
using Application.LogicInterfaces;
using Data.DAOs;
using Domain.DTOs.Team;

[TestClass]
public class TeamUnitTests
{
    [TestMethod]
    public void EmptyNameTest()
    {
        //Initializing Logic class
        ITeamLogic logic = new TeamLogic(new TeamDAO(), new MemberDAO());
        
        //Initializing new object with incorrect dates
        TeamCreateDTO team = new TeamCreateDTO("", 192);

        //Assert
        var expectedErrorMessage = "Name must not be empty";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(team));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    }
    
    [TestMethod]
    public void InvalidNameTest()
    {
        //Initializing Logic class
        ITeamLogic logic = new TeamLogic(new TeamDAO(), new MemberDAO());
        
        //Initializing new object with incorrect dates
        TeamCreateDTO team = new TeamCreateDTO("bruh", 192);

        //Assert
        var expectedErrorMessage = "Name must be between 5 and 20 characters.";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(team));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    }
    
    [TestMethod]
    public void NonExistantLeaderTest()
    {
        //Initializing Logic class
        ITeamLogic logic = new TeamLogic(new TeamDAO(), new MemberDAO());
        
        //Initializing new object with incorrect dates
        TeamCreateDTO team = new TeamCreateDTO("Test team", 1);

        //Assert
        var expectedErrorMessage = $"Member not found";
        Task<RpcException> exception = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(team));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Status.Detail);
    }
    
    [TestMethod]
    public void CreatingTeamTest()
    {
        //Initializing Logic class
        ITeamLogic logic = new TeamLogic(new TeamDAO(), new MemberDAO());
        
        //Initializing new object with incorrect dates
        TeamCreateDTO team = new TeamCreateDTO("asdhgf", 191);

        //Assert
        var expectedErrorMessage = "Member not found";
        Task<RpcException> exception = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(team));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Status.Detail);
    }
    [TestMethod]
    public void CreatingTeamNameTest()
    {
        //Initializing Logic class
        ITeamLogic logic = new TeamLogic(new TeamDAO(), new MemberDAO());
        
        //Initializing new object with incorrect dates
        TeamCreateDTO team = new TeamCreateDTO("Test team", 192);

        //Assert
        var expectedErrorMessage = "Name is in use";
        Task<RpcException> exception = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(team));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Status.Detail);
    }
}

// public string Name { get; }
// public int TeamLeaderId { get; }