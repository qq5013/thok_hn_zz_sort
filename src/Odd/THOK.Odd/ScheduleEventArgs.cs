using System;
using System.Collections.Generic;
using System.Text;

namespace THOK.Odd
{
    internal delegate void ProcessEventHandler(object sender, ProcessEventArgs e);

    internal enum OptimizeStatus { WAITING, PROCESSING, COMPLETE, ERROR };

    internal class ProcessEventArgs
    {
        private int scheduleStep = 0;

        private string stepName = "";

        private int completeCount;

        private int totalCount;

        private string message;

        private OptimizeStatus optimizeStatus = OptimizeStatus.WAITING;

        private bool isContinure = true;

        public bool IsContinure
        {
            get { return isContinure; }
            set { isContinure = value; }
        }

        public int ScheduleStep
        {
            get
            {
                return scheduleStep;
            }
        }

        public string StepName
        {
            get
            {
                return stepName;
            }
        }

        public int CompleteCount
        {
            get
            {
                return completeCount;
            }
        }

        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = null;
            }
        }

        public OptimizeStatus OptimizeStatus
        {
            get
            {
                return optimizeStatus;
            }
            set
            {
                optimizeStatus = value;
            }
        }

        public ProcessEventArgs(OptimizeStatus optimizeStatus)
        {
            this.optimizeStatus = optimizeStatus;
        }

        public ProcessEventArgs(OptimizeStatus optimizeStatus, string message)
        {
            this.optimizeStatus = optimizeStatus;
            this.message = message;
        }

        public ProcessEventArgs(int scheduleStep, string stepName, int completeCount, int totalCount)
        {
            this.scheduleStep = scheduleStep;
            this.stepName = stepName;
            this.completeCount = completeCount;
            this.totalCount = totalCount;
            this.optimizeStatus = OptimizeStatus.PROCESSING;
        }
    }
}
