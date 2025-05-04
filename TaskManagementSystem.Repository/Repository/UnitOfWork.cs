using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskManagementSystem.Common.BusinessEntities;
using System;
using static TaskManagementSystem.Common.Utility.Enumeration;
using System.Diagnostics;


using TaskManagementSystem.Repository.Models;
using TaskManagementSystem.Repository.Repository;

namespace TaskManagementSystem.Repository.Repository
{
    public class UnitOfWork : IDisposable
    {
        private TestDbContext context = new TestDbContext();

        private GenericRepository<HiteshTaskAssignTask> _HiteshTaskAssignTask;
        private GenericRepository<HiteshTaskProject> _HiteshTaskProject;
        private GenericRepository<HiteshTaskProjectAccess> _HiteshTaskProjectAccess;
        private GenericRepository<HiteshTaskRole> _HiteshTaskRole;
        private GenericRepository<HiteshTaskUserMaster> _HiteshTaskUserMaster;
        private GenericRepository<HiteshTaskDocumentSave> _HiteshTaskDocumentSave;
        private GenericRepository<HiteshTaskMessage> _HiteshTaskMessage;
        //private GenericRepository<HiteshCourtSlotDetail> _HiteshCourtSlotDetail;
        //private GenericRepository<HiteshCricketCourtBooking> _HiteshCricketCourtBooking;

        public GenericRepository<HiteshTaskAssignTask> HiteshTaskAssignTask
        {
            get
            {
                if (_HiteshTaskAssignTask == null)
                    _HiteshTaskAssignTask = new GenericRepository<HiteshTaskAssignTask>(context);
                return _HiteshTaskAssignTask;
            }
        }

        public GenericRepository<HiteshTaskMessage> HiteshTaskMessage
        {
            get
            {
                if (_HiteshTaskMessage == null)
                    _HiteshTaskMessage = new GenericRepository<HiteshTaskMessage>(context);
                return _HiteshTaskMessage;
            }
        }

        public GenericRepository<HiteshTaskDocumentSave> HiteshTaskDocumentSave
        {
            get
            {
                if (_HiteshTaskDocumentSave == null)
                    _HiteshTaskDocumentSave = new GenericRepository<HiteshTaskDocumentSave>(context);
                return _HiteshTaskDocumentSave;
            }
        }

        public GenericRepository<HiteshTaskProject> HiteshTaskProject
        {
            get
            {
                if (_HiteshTaskProject == null)
                    _HiteshTaskProject = new GenericRepository<HiteshTaskProject>(context);
                return _HiteshTaskProject;
            }
        }

        public GenericRepository<HiteshTaskProjectAccess> HiteshTaskProjectAccess
        {
            get
            {
                if (_HiteshTaskProjectAccess == null)
                    _HiteshTaskProjectAccess = new GenericRepository<HiteshTaskProjectAccess>(context);
                return _HiteshTaskProjectAccess;
            }
        }

        public GenericRepository<HiteshTaskRole> HiteshTaskRole
        {
            get
            {
                if (_HiteshTaskRole == null)
                    _HiteshTaskRole = new GenericRepository<HiteshTaskRole>(context);
                return _HiteshTaskRole;
            }
        }

        public GenericRepository<HiteshTaskUserMaster> HiteshTaskUserMaster
        {
            get
            {
                if (_HiteshTaskUserMaster == null)
                    _HiteshTaskUserMaster = new GenericRepository<HiteshTaskUserMaster>(context);
                return _HiteshTaskUserMaster;
            }
        }

        //public GenericRepository<HiteshCricketRole> HiteshCricketRole
        //{
        //    get
        //    {
        //        if (_HiteshCricketRole == null)
        //            _HiteshCricketRole = new GenericRepository<HiteshCricketRole>(context);
        //        return _HiteshCricketRole;
        //    }
        //}

        //public GenericRepository<HiteshCricketUserMaster> HiteshCricketUserMaster
        //{
        //    get
        //    {
        //        if (_HiteshCricketUserMaster == null)
        //            _HiteshCricketUserMaster = new GenericRepository<HiteshCricketUserMaster>(context);
        //        return _HiteshCricketUserMaster;
        //    }
        //}
        //public GenericRepository<HiteshCity> HiteshCity
        //{
        //    get
        //    {
        //        if (_HiteshCity == null)
        //            _HiteshCity = new GenericRepository<HiteshCity>(context);
        //        return _HiteshCity;
        //    }
        //}
        #region Save and Dispose
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
