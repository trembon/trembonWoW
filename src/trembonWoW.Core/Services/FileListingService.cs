using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Database;
using trembonWoW.Database.Entities;

namespace trembonWoW.Core.Services;

public interface IFileListingService
{
    Task<List<ListedFile>> GetFiles(string category);
}
public class FileListingService(DefaultContext defaultContext) : IFileListingService
{
    public async Task<List<ListedFile>> GetFiles(string category)
    {
        return await defaultContext
            .ListedFiles
            .Where(x => x.Category == category)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Filename)
            .ToListAsync();
    }
}
