﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public bool RequireConfirmation { get; set; }

        public EventScheduleInfoDtoBase Schedule { get; set; } = new OneTimeScheduleInfoDto();
        public bool Suspended { get; set; }
    }
}
