﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using Microsoft.Rest;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using System.Linq;

namespace Pssg.Interfaces.FlatFileModels
{
    [FixedLengthRecord()]
    public class MedicalUpdate
    {
        // LNUM Driver License Number Char(7)
        [FieldFixedLength(7)]
        public string LicenseNumber { get; set; }

        // SURN Surname Char(35)
        [FieldFixedLength(35)]
        public string Surname { get; set; }

        // MDSP Medical Disposition Char(1) (P = Clean Pass, J = Adjudication)
        [FieldFixedLength(1)]
        public string MedicalDisposition { get; set; }

        [FieldFixedLength(10)]
        // MIDT DFCMS Medical Issue Date Char(10)
        public string MedicalIssueDate { get; set; }
    }
}
