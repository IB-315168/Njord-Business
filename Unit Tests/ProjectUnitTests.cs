using Application.Logic;
using Application.LogicInterfaces;
using Data.DAOs;
using Domain.DTOs.Project;
using Grpc.Core;


namespace Unit_Tests;

[TestClass]
public class ProjectUnitTests
{
    [TestMethod]
    public void EmptyNameTest()
    {
        //Initializing Logic class
        IProjectLogic logic = new ProjectLogic(new ProjectDAO(), new TeamDAO(), new LogBookDAO());
        
        //Initializing new object with incorrect dates
        ProjectCreateDTO project = new ProjectCreateDTO()
        {
            Name="",
            TeamId = 198,
            Deadline = new DateTime(1, 1, 1, 1, 1, 1)
        };
        
        //Assert
        var expectedErrorMessage = "Name must not be empty";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(project));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    }

    [TestMethod]
    public void OutOfBoundsNameTest()
    {
        //Initializing Logic class
        IProjectLogic logic = new ProjectLogic(new ProjectDAO(), new TeamDAO(), new LogBookDAO());
        
        //Initializing new object with incorrect dates
        ProjectCreateDTO project = new ProjectCreateDTO()
        {
            Name="bruh bruh bruh bruh bruh bruh bruh bruh bruh bruh ",
            TeamId = 198,
            Deadline = new DateTime(1, 1, 1, 1, 1, 1)
        };
        
        //Assert
        var expectedErrorMessage = "Name must be between 5 and 25 characters.";
        Task<Exception> exception = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(project));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Message);
    }

    [TestMethod]
    public void TeamNotFoundTest()
    {
        //Initializing Logic class
        IProjectLogic logic = new ProjectLogic(new ProjectDAO(), new TeamDAO(), new LogBookDAO());
        
        //Initializing new object with incorrect dates
        ProjectCreateDTO project = new ProjectCreateDTO()
        {
            Name="Test name",
            TeamId = 0,
            Deadline = new DateTime(1, 1, 1, 1, 1, 1)
        };
        
        //Assert
        var expectedErrorMessage = "Team not found";
        Task<RpcException> exception = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(project));
        Assert.AreEqual(expectedErrorMessage, exception.Result.Status.Detail);
    }

    [TestMethod]
    public void InvalidDeadlineTest()
    {
        try
        {
            //Initializing Logic class
            IProjectLogic logic = new ProjectLogic(new ProjectDAO(), new TeamDAO(), new LogBookDAO());

            //Initializing new object with incorrect dates
            ProjectCreateDTO project = new ProjectCreateDTO()
            {
                Name = "Test name",
                TeamId = 198,
                Deadline = new DateTime(0, 0, 0, 0, 0, 0)
            };

            logic.CreateAsync(project);
        }
        catch (ArgumentOutOfRangeException e)
        {
            //Assert
            StringAssert.Contains(e.Message, "parameters describe an un-representable DateTime");
        }
    }

    [TestMethod]
    public void CreatingProjectTest()
    {
        //Initializing Logic class
        IProjectLogic logic = new ProjectLogic(new ProjectDAO(), new TeamDAO(), new LogBookDAO());

        //Initializing new object with incorrect dates
        ProjectCreateDTO project = new ProjectCreateDTO()
        {
            Name = "Test name",
            TeamId = 198,
            Deadline = new DateTime(1, 1, 1, 1, 1, 1)
        };

        logic.CreateAsync(project); 
    }
}