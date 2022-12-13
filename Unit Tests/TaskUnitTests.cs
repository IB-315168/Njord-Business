using Application.DAOInterfaces;
using Application.Logic;
using Data.Converters;
using Data.DAOs;
using Domain.DTOs.Task;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcNjordClient.SpecificDateTime;
using GrpcNjordClient.Task;

namespace Unit_Tests;

    [TestClass]
    public class TaskUnitTests
    {
        [TestMethod]
        public void ValidateTitleEmpty()
        {
            ITaskDAO taskDao= new TaskDAO();
            IProjectDAO projectDao=new ProjectDAO();
            IMemberDAO memberDao=new MemberDAO();
            TaskLogic logic = new TaskLogic(taskDao,projectDao,memberDao);

            TaskCreateDTO taskCreateDto = new TaskCreateDTO()
            {
                projectAssigned = 293,
                Title = "",
                Description = "test" ,
                Status = "C - Completed",
                CreationDate = DateTime.Now
            };
            var expectedErrorMessage = "Title must not be empty";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(taskCreateDto));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }
        [TestMethod]
        public void ValidateTitleLength()
        {
            ITaskDAO taskDao= new TaskDAO();
            IProjectDAO projectDao=new ProjectDAO();
            IMemberDAO memberDao=new MemberDAO();
            TaskLogic logic = new TaskLogic(taskDao,projectDao,memberDao);

            TaskCreateDTO taskCreateDto = new TaskCreateDTO()
            {
                projectAssigned = 293,
                Title = "t",
                Description = "test" ,
                Status = "C - Completed",
                CreationDate = DateTime.Now
            };

            var expectedErrorMessage = "Title must be between 3 and 100 characters.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(taskCreateDto));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }
        [TestMethod]
        public void ValidateDescription()
        {
            ITaskDAO taskDao= new TaskDAO();
            IProjectDAO projectDao=new ProjectDAO();
            IMemberDAO memberDao=new MemberDAO();
            TaskLogic logic = new TaskLogic(taskDao,projectDao,memberDao);

            TaskCreateDTO taskCreateDto = new TaskCreateDTO()
            {
                projectAssigned = 293,
                Title = "title",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, " +
                "when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into " +
                "electronic typesetting, remaining essentially unchanged. Lorem Ipsum is simply dummy text of the printing and typesetting industry.Lorem Ipsum is simply dummy text of " +
                "Lorem Ipsum is simply dummy text of the printing and typesetting industry.Lorem Ipsum is simply dummy text of the printing and typesetting industry.the printing and typesetting industry.Lorem Ipsum is simply dummy text of the printing and typesetting industry." ,
                Status = "C - Completed",
                CreationDate = DateTime.Now
            };

            var expectedErrorMessage = "Description cannot be longer than 500 characters.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(taskCreateDto));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }
        [TestMethod]
        public void UpdateStatus()
        {
            ITaskDAO taskDao= new TaskDAO();
            IProjectDAO projectDao=new ProjectDAO();
            IMemberDAO memberDao=new MemberDAO();
            TaskLogic logic = new TaskLogic(taskDao,projectDao,memberDao);

            TaskUpdateDTO taskUpdateDto = new TaskUpdateDTO(207)
            {
                memberAssigned = new MemberEntity(){Availability = new List<AvailabilityEntity>(),Email = "test@test.com", FullName = "full name", Password = "password", UserName = "username", Id = 139},
                Title = "test",
                Description = "test" ,
                Status = "test",
                TimeEstimation = DateTime.Now
            };

            var expectedErrorMessage = "Status can only be Todo, In-progress or Completed";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.UpdateAsync(taskUpdateDto));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }
        [TestMethod]
        public void UpdateTime()
        {
            ITaskDAO taskDao = new TaskDAO();
            IProjectDAO projectDao = new ProjectDAO();
            IMemberDAO memberDao = new MemberDAO();
            TaskLogic logic = new TaskLogic(taskDao,projectDao,memberDao);
            SpecificDateTime specificDateTime = new SpecificDateTime();
            specificDateTime.Year = 1;
            specificDateTime.Month = 1;
            specificDateTime.Day = -1;
            specificDateTime.Hour = -1;
            specificDateTime.Minute = -1;
            TaskUpdateDTO dto = new TaskUpdateDTO(207)
            {
                memberAssigned = new MemberEntity() { Availability = new List<AvailabilityEntity>(), Email = "test@test.com", FullName = "full name", Password = "password", UserName = "username", Id = 139 },
                Title = "test",
                Description = "test",
                Status = "Completed"
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => SpecificDateTimeConverter.convertToDateTime(specificDateTime));
        }
    }