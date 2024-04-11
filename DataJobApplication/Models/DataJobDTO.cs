using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataJobApplication.Models
{
    public class DataJobDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string FilePathToProcess { get; set; }
        public DataJobStatus? Status { get; set; } = DataJobStatus.New;
        public IEnumerable<string> Results { get; set; } = new List<string>();
        public IEnumerable<Link> Links { get; set; } = new List<Link>();
    }
}