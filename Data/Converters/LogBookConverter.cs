using Data.DAOs;
using Domain.Models;
using GrpcNjordClient.LogBook;

namespace Data.Converters;

public class LogBookConverter
{
    public static LogBookEntity ConvertToLogBookEntity(LogBookGrpc logbook)
    {
        List<LogBookEntryEntity> entries = new List<LogBookEntryEntity>();

        foreach (LogBookEntryGrpc entry in logbook.Logbookentries)
        {
            entries.Add(new LogBookEntryEntity(entry.Id,entry.Assignedlogbook,MeetingDAO.ConvertToMeetingEntity(entry.Assignedmeeting),entry.Contents));
        }

        return new LogBookEntity(logbook.Id, ProjectConverter.ConvertToProjectEntity(logbook.Projectassigned))
        {
            Entries = entries
        };
    }
    
    public static LogBookEntryGrpc convertToEntry(LogBookEntryEntity entryEntity)
    {
        return new LogBookEntryGrpc()
        {
            Id = entryEntity.Id,
            Assignedlogbook = entryEntity.IdLogBook,
            Assignedmeeting = MeetingDAO.ConvertToMeetingGrpc(entryEntity.IdMeeting),
            Contents = entryEntity.content
        };
    }

    public static ICollection<LogBookEntryGrpc> convertToEntries(ICollection<LogBookEntryEntity> dtoEntries)
    {
        ICollection<LogBookEntryGrpc> entries = new List<LogBookEntryGrpc>();

        foreach(LogBookEntryEntity entity in dtoEntries)
        {
            entries.Add(LogBookConverter.convertToEntry(entity));
        }

        return entries;
    }
}