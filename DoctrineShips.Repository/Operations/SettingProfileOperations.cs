namespace DoctrineShips.Repository.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;
    using Tools;

    internal sealed class SettingProfileOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal SettingProfileOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteSettingProfile(int settingProfileId)
        {
            this.unitOfWork.Repository<SettingProfile>().Delete(settingProfileId);
        }

        internal void UpdateSettingProfile(SettingProfile settingProfile)
        {
            settingProfile.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<SettingProfile>().Update(settingProfile);
        }

        internal SettingProfile AddSettingProfile(SettingProfile settingProfile)
        {
            this.unitOfWork.Repository<SettingProfile>().Insert(settingProfile);
            return settingProfile;
        }

        internal SettingProfile CreateSettingProfile(SettingProfile settingProfile)
        {
            settingProfile.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<SettingProfile>().Insert(settingProfile);
            return settingProfile;
        }

        internal SettingProfile GetSettingProfile(int settingProfileId)
        {
            return this.unitOfWork.Repository<SettingProfile>().Find(settingProfileId);
        }

        internal SettingProfile GetSettingProfileReadOnly(int settingProfileId)
        {
            return this.unitOfWork.Repository<SettingProfile>()
                                            .Query()
                                            .TrackingOff()
                                            .Filter(x => x.SettingProfileId == settingProfileId)
                                            .Get()
                                            .FirstOrDefault();
        }

        internal SettingProfile GetSettingProfileForAccount(int accountId)
        {
            var settingProfile = this.unitOfWork.Repository<SettingProfile>()
                      .Query()
                      .Filter(q => q.AccountId == accountId)
                      .Get()
                      .FirstOrDefault();

            return settingProfile;
        }

        internal IEnumerable<SettingProfile> GetSettingProfiles()
        {
            var settingProfiles = this.unitOfWork.Repository<SettingProfile>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.SettingProfileId)
                              .ToList();

            return settingProfiles;
        }
    }
}