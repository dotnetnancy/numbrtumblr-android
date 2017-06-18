using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;

namespace NumbrTumblr.DataServices
{
    public class LocalStorageFileManager
    {
        private static async Task<IFolder> NavigateToFolder(string targetFolder)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync(targetFolder,
                CreationCollisionOption.OpenIfExists);

            return folder;
        }

        //private async Task StoreImagesLocallyAndUpdatePath(IFolder folder, IEnumerable<Models.Company> companies)
        //{
        //    foreach (var company in companies)
        //    {
        //        var file = await folder.CreateFileAsync(company.Name + ".jpg", CreationCollisionOption.ReplaceExisting);
        //        using (var fileHandler = await file.OpenAsync(FileAccess.ReadAndWrite))
        //        {
        //            var httpResponse = await _httpClient.GetAsync(company.ImageUri);
        //            byte[] imageBuffer = await httpResponse.Content.ReadAsByteArrayAsync();
        //            await fileHandler.WriteAsync(imageBuffer, 0, imageBuffer.Length);

        //            company.ImageUri = file.Path;
        //        }
        //    }
        //}

        //private async Task<IEnumerable<Models.Company>> ReadCompaniesFromFile()
        //{
        //    var folder = await NavigateToFolder(CompaniesFolder);

        //    if ((await folder.CheckExistsAsync(CompaniesFileName)) == ExistenceCheckResult.NotFound)
        //    {
        //        return new List<Models.Company>();
        //    }

        //    IFile file = await folder.GetFileAsync(CompaniesFileName);
        //    var jsonCompanies = await file.ReadAllTextAsync();

        //    if (string.IsNullOrEmpty(jsonCompanies)) return new List<Models.Company>();

        //    var companies = JsonConvert.DeserializeObject<IEnumerable<Models.Company>>(jsonCompanies);

        //    return companies;
        //}

        //public async Task<IEnumerable<Models.Company>> GetCompanies()
        //{
        //    return _companies ?? (_companies = await ReadCompaniesFromFile());
        //}

        //private static async Task SerializeCompanies(IFolder folder, ICollection<Models.Company> companies)
        //{
        //    IFile file = await folder.CreateFileAsync(CompaniesFileName, CreationCollisionOption.ReplaceExisting);
        //    var companiesString = JsonConvert.SerializeObject(companies);
        //    await file.WriteAllTextAsync(companiesString);
        //}

        public async Task CreateRealFileAsync()
        {
            // get hold of the file system
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file
            IFile file = await folder.CreateFileAsync("MyFile.txt", CreationCollisionOption.ReplaceExisting);

            // populate the file with some text
            await file.WriteAllTextAsync("Sample Text...");
        }
    }
}
