﻿using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using MSS1.Repository;

namespace MSS1.Services
{
    public class StudentService: IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<StudentResponseDTO>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllStudentsAsync();
            return students.Select(s => new StudentResponseDTO
            {
                StudentId = s.StudentId,
                FullName = s.User.FullName,
                Email = s.User.Email,
                Address = s.User.Address,
                MobileNumber = s.User.MobileNumber,
                Gender = s.User.Gender,
                RegistrationFee = s.RegistrationFee,
               // IsRegistrationFeePaid = s.IsRegistrationFeePaid,
                NICNumber = s.User.NICNumber,
                DateOfBirth = s.User.DateOfBirth,
                ProfilePicture = s.User.ProfilePicture,

                // Include NICNumber here
            }).ToList();
        }


        // Get Student By Id
        public async Task<StudentResponseDTO> GetStudentByIdAsync(int studentId)
        {
            var student = await _repository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new ArgumentException("Student not found.");

            return new StudentResponseDTO
            {
                StudentId = student.StudentId,
                FullName = student.User.FullName,
                Email = student.User.Email,
                Address = student.User.Address,
                MobileNumber = student.User.MobileNumber,
                Gender = student.User.Gender,
                ProfilePicture = student.User.ProfilePicture,
               // RegistrationFee = student.RegistrationFee,
                //IsRegistrationFeePaid = student.IsRegistrationFeePaid,
                NICNumber = student.User.NICNumber,
                DateOfBirth= student.User.DateOfBirth,
            };
        }

        //Get Student By NIC
        public async Task<StudentResponseDTO> GetStudentByNICNumberAsync(string nicNumber)
        {
            var student = await _repository.GetStudentByNICNumberAsync(nicNumber);
            if (student == null)
                throw new ArgumentException("Student not found.");

            return new StudentResponseDTO
            {
                FullName = student.User.FullName,
                Email = student.User.Email,
                Address = student.User.Address,
                MobileNumber = student.User.MobileNumber,
                Gender = student.User.Gender,
                NICNumber = student.User.NICNumber,
                DateOfBirth = student.User.DateOfBirth,
                ProfilePicture= student.User.ProfilePicture,
            };
        }
        // Delete Student By Id
        public async Task DeleteStudentAsync(int studentId)
        {
            await _repository.DeleteStudentAsync(studentId);
        }

        // Update Student
        public async Task<StudentResponseDTO> UpdateStudentAsync(int studentId, UpdateStudentRequestDTO request)
        {
            var updatedStudent = await _repository.UpdateStudentAsync(new Student
            {
                StudentId = studentId,
                User = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Address = request.Address,
                    MobileNumber = request.MobileNumber,
                    Gender = request.Gender,
                    NICNumber = request.NICNumber,
                    DateOfBirth= request.DateOfBirth,
                    ProfilePicture = request.ProfilePicture
                },
                RegistrationFee = request.RegistrationFee,
                IsRegistrationFeePaid = request.IsRegistrationFeePaid
            });

            return new StudentResponseDTO
            {
                //StudentId = updatedStudent.StudentId,
                FullName = updatedStudent.User.FullName,
                Email = updatedStudent.User.Email,
                Address = updatedStudent.User.Address,
                MobileNumber = updatedStudent.User.MobileNumber,
                Gender = updatedStudent.User.Gender,
                ProfilePicture = updatedStudent.User.ProfilePicture,
               // RegistrationFee = updatedStudent.RegistrationFee,
               // IsRegistrationFeePaid = updatedStudent.IsRegistrationFeePaid,
                //DateOfBirth = updatedStudent.DateOfBirth,

            };
        }
        public async Task<int> GetTotalStudentCountAsync()
        {
            return await _repository.GetTotalStudentCountAsync();
        }
        public async Task<decimal> GetCumulativeRegistrationFeeAsync()
        {
            return await _repository.GetCumulativeRegistrationFeeAsync();
        }



    }
}
