using System;
using Android.Widget;
using Thepagedot.Rhome.Base.Models;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Thepagedot.Rhome.App.Droid;
using Android.App;

namespace Thepagedot.Rhome.App.Droid
{
    public class RoomAdapter
    {
        public static View GetNoteView(int position, Room room, View convertView)
        {
            var view = convertView;
            if (view == null)
                view = LayoutInflater.From(Application.Context).Inflate(Resource.Layout.RoomItem, null);

            view.FindViewById<TextView>(Resource.Id.tvRoomName).Text = room.Name;

            return view;
        }
    }
}