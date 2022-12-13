using Application.DAOInterfaces;
using Application.Logic;
using Data.DAOs;
using Domain.DTOs.LogBook;
using Domain.DTOs.Member;
using Grpc.Core;

namespace Unit_Tests;

[TestClass]
public class LogbookUnitTests
{
    [TestMethod]
    public void LogbookCreateAsync()
    {
        ILogBookDAO logbookDao =new LogBookDAO();
        IProjectDAO projectDao=new ProjectDAO();
        LogBookLogic logic = new LogBookLogic(logbookDao,projectDao);
        LogBookCreateDTO logBookCreateDTO = new LogBookCreateDTO()
        {
           ProjectId = 0
        };
        var expectedErrorMessage = "Project not found";
        Task<RpcException> ex = Assert.ThrowsExceptionAsync<RpcException>(() => logic.CreateAsync(logBookCreateDTO));
        Assert.AreEqual(expectedErrorMessage, ex.Result.Status.Detail);
    }
    [TestMethod]
    public void LogbookDeleteAsync()
    {
        ILogBookDAO logbookDao =new LogBookDAO();
        IProjectDAO projectDao=new ProjectDAO();
        LogBookLogic logic = new LogBookLogic(logbookDao,projectDao);
        
        var expectedErrorMessage = "Logbook not found";
        Task<RpcException> ex = Assert.ThrowsExceptionAsync<RpcException>(() => logic.DeleteAsync(0));
        Assert.AreEqual(expectedErrorMessage, ex.Result.Status.Detail);
    }
    [TestMethod]
    public void LogbookUpdateAsync()
    {
        ILogBookDAO logbookDao =new LogBookDAO();
        IProjectDAO projectDao=new ProjectDAO();
        LogBookLogic logic = new LogBookLogic(logbookDao,projectDao);
        
        var expectedErrorMessage = "Logbook not found";
        Task<RpcException> ex = Assert.ThrowsExceptionAsync<RpcException>(() => logic.DeleteAsync(0));
        Assert.AreEqual(expectedErrorMessage, ex.Result.Status.Detail);
    }
    [TestMethod]
    public void LogbookGetByIdAsyncAsync()
    {
        ILogBookDAO logbookDao =new LogBookDAO();
        IProjectDAO projectDao=new ProjectDAO();
        LogBookLogic logic = new LogBookLogic(logbookDao,projectDao);
        
        var expectedErrorMessage = "Logbook not found";
        Task<RpcException> ex = Assert.ThrowsExceptionAsync<RpcException>(() => logic.DeleteAsync(0));
        Assert.AreEqual(expectedErrorMessage, ex.Result.Status.Detail);
    }
    [TestMethod]
    public void LogbookGetByProjectIdAsync()
    {
        ILogBookDAO logbookDao =new LogBookDAO();
        IProjectDAO projectDao=new ProjectDAO();
        LogBookLogic logic = new LogBookLogic(logbookDao,projectDao);
        
        var expectedErrorMessage = "Logbook not found";
        Task<RpcException> ex = Assert.ThrowsExceptionAsync<RpcException>(() => logic.DeleteAsync(0));
        Assert.AreEqual(expectedErrorMessage, ex.Result.Status.Detail);
    }
}