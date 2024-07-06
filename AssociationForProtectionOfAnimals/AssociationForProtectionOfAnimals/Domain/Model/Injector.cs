using AssociationForProtectionOfAnimals.Controller;
using System;
using System.Collections.Generic;
using AssociationForProtectionOfAnimals.Domain.IRepository;
using AssociationForProtectionOfAnimals.Repository;

namespace AssociationForProtectionOfAnimals.Domain.Model
{
    public static class Injector
    {
        private static Dictionary<Type, Lazy<object>> _implementations = new Dictionary<Type, Lazy<object>>
        {
            { typeof(IAccountRepo), new Lazy<object>(() => new AccountRepo()) },
            { typeof(IPlaceRepo), new Lazy<object>(() => new PlaceRepo()) },
            { typeof(IRegisteredUserRepo), new Lazy<object>(() => new RegisteredUserRepo()) },
            { typeof(RegisteredUserController), new Lazy<object>(() => new RegisteredUserController()) },
            { typeof(IVolunteerRepo), new Lazy<object>(() => new VolunteerRepo()) },
            { typeof(VolunteerController), new Lazy<object>(() => new VolunteerController()) },
            { typeof(IAdminRepo), new Lazy<object>(() => new AdminRepo()) },
            { typeof(AdministratorController), new Lazy<object>(() => new AdministratorController()) },
            { typeof(IAnimalRepo), new Lazy<object>(() => new AnimalRepo()) },
            { typeof(PostController), new Lazy<object>(() => new PostController()) },
            { typeof(IPostRepository), new Lazy<object>(() => new PostRepository()) },
            { typeof(ICommentRepository), new Lazy<object>(() => new CommentRepository()) },
            { typeof(CommentController), new Lazy<object>(() => new CommentController()) },
            { typeof(IRequestRepo), new Lazy<object>(() => new RequestRepo()) },
            { typeof(IAdoptionRequestRepo), new Lazy<object>(() => new AdoptionRequestRepo()) },
            { typeof(ITemporaryShelterRequestRepo), new Lazy<object>(() => new TemporaryShelterRequestRepo()) },
            { typeof(RequestController), new Lazy<object>(() => new RequestController()) },
            { typeof(IDonationRepository), new Lazy<object>(() => new DonationRepository()) },
            { typeof(DonationController), new Lazy<object>(() => new DonationController()) },

        };

        static Injector()
        {
            /*Data.AppDbContext appDbContext = new Data.AppDbContext();   
            _implementations.Add(typeof(IPenaltyPointRepo), new Lazy<object>(() => new PenaltyPointRepo()));
            _implementations.Add(typeof(PenaltyPointController), new Lazy<object>(() => new PenaltyPointController()));
            _implementations.Add(typeof(ReportController), new Lazy<object>(() => new ReportController()));
            _implementations.Add(typeof(IExamTermDbRepo), new Lazy<object>(() => new ExamTermDbRepo(appDbContext))); //
            _implementations.Add(typeof(ICourseDbRepo), new Lazy<object>(() => new CourseDbRepo(appDbContext)));     //
            _implementations.Add(typeof(IAdminDbRepo), new Lazy<object>(() => new AdminDbRepo(appDbContext)));*/

        }

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type].Value;
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
