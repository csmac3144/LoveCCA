﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Models
{
    public enum OrderStatus
    {
        None, Pending, Completed
    }
    public class Day
    {
        public Day()
        {
            OrderStatus = OrderStatus.None;

        }
        public DateTime Date { get; set; }
        public string DateLabel {  
            get
            {
                return Date.ToString("M");
            }
        }
        public string DayOfWeekLabel {  
            get
            {
                return Date.DayOfWeek.ToString();
            }
        }
        public string Description { get; set; }
        public bool IsNotSchoolDay { get; set; }
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; internal set; }
        public string OrderChild { get; set; }
        public string OrderProductType { get; set; }
        private bool _isPending;
        public bool IsPending { 
            get
            {
                return OrderStatus == OrderStatus.Pending;
            }
            set
            {
                _isPending = value;
                if (_isPending)
                    OrderStatus = OrderStatus.Pending;
                else
                    OrderStatus = OrderStatus.None;
            }
        }

    }
}
