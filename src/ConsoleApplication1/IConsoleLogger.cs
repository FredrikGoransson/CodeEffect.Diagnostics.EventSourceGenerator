﻿using System;

namespace ConsoleApplication1
{
    public interface IConsoleLogger
    {
        //void SayHello(string message);
        //void Message(string message);
        //void Error(Exception exception);

        void SayGoodbye(string goodbye, DateTime nightTime);
    }

    //public interface IMongoLogger
    //{
    //    void Hello();
    //}

    //public interface IDomainLogger
    //{
    //    void CompanyCreated(string name, string orgno);
    //}
}