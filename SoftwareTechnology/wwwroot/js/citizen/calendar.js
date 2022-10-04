
    document.addEventListener("DOMContentLoaded", function () {
        var Calendar = FullCalendar.Calendar;
        var calendarEl = document.getElementById("calendar");

        var calendar = new Calendar(calendarEl, {
        plugins: ["timeGrid", "interaction"],
            header: {
        left: "prev,next today",
                center: "title",
                right: "timeGridFourDay"
            },
            defaultView: "timeGridFourDay",
            views: {
        timeGridFourDay: {
        type: "timeGrid",
                    duration: {days: 7 },
                    buttonText: "7 day"
                }
            },
            slotLabelFormat: {
        hour: "numeric",
                minute: "2-digit",
                omitZeroMinute: true,
                meridiem: "short"
            },
            @if (Model.citizen.Dose == 0)
            {

                  @:validRange: function () {

                    @: today = new Date();

                    @: today.setDate(today.getDate() + 1);
                    @: endDate = new Date(today);
                    @: endDate.setDate(endDate.getDate() + 14);

                    @: return {

                        @: start: today,
                        @: end: endDate

                    @: };
                @: },
                @:selectable: true,




            }
            else if (Model.citizen.Dose == 1)
            {


                @:validRange: function () {

                 @:var startDate = new Date("@httpContext.HttpContext.Session.GetString("firstDose")");

                 @: startDate.setDate(startDate.getDate() + 28);
                 @: endDate = new Date(startDate);
                 @: endDate.setDate(endDate.getDate() + 14);


                 @: return {
                    @: start: startDate,
                    @: end: endDate
                    @: };
                @: },
                @:selectable: true,

            }
            else
            {
                 @:selectable: false,

            }

            locale: 'gr',
            allDaySlot: false,
            firstDay: 1,
            minTime: "08:00:00",
            maxTime: "18:00:00",
            contentHeight: "auto",
            slotDuration: "00:30:00",

            displayEventTime: true,
            eventColor: '#b7ff00',
            select: function (info) {
                if (calendar.getEventById("Test") != null) {
        calendar.getEventById("Test").remove();

                }
                //console.log(info);
                calendar.addEvent({title: "Test", start: info.start, end: info.end, type: "Test", id: "Test" });

                calendar.unselect();


            },
            eventClick: function (eventClickInfo) {


                if (eventClickInfo.event.extendedProps.type != "busy") {
        eventClickInfo.event.remove();

                }

            },

            selectOverlap: false,
            selectAllow: function (selectInfo) {

                var stM = moment(selectInfo.start);
                var enM = moment(selectInfo.end);
                var diff = enM.diff(stM, "minutes");
                console.log(diff);
                if (diff > 30) {
                    return false;
                }
                return true;
            },

            eventSources: [
                {
        id: "busy", backgroundColor: "red",
                    events: [

                      @if (Model.appointmentList != null)
                      {
                    @foreach (var item in Model.appointmentList) {

                                @:{
                                        @: title: ("Existing event"),
                                        @: start: ('@item.Date.ToString("yyyy-MM-dd") @item.Time.ToString()'),
                                        @: end: ('@item.Date.ToString("yyyy-MM-dd") @item.Time.Add(new TimeSpan(0, 30, 0)).ToString()'),
                                        @: type: ("busy")
                                @:}
                            if (item != Model.appointmentList.Last())
                        {
                                @: ,
                            }

                    }
                      }

                    ]
                }
            ]



        });

        calendar.render();
        function getEvents() {

            @if (httpContext.HttpContext.Session.GetString("firstDose") == null)
            {

                @:document.getElementById("firstdose").value = toIsoString(calendar.getEventById("Test").start);
            }
            else if (httpContext.HttpContext.Session.GetString("secDose") == null)
            {


                @:document.getElementById("firstdose").value = "@httpContext.HttpContext.Session.GetString("firstDose")";
                @:document.getElementById("secdose").value = toIsoString(calendar.getEventById("Test").start);



            }

            @*location.reload();*@

        }



        document.getElementById('getevent').addEventListener('click', getEvents);
    });

    function toIsoString(date) {
        var tzo = -date.getTimezoneOffset(),
            dif = tzo >= 0 ? '+' : '-',
            pad = function (num) {
                return (num < 10 ? '0' : '') + num;
            };

        return date.getFullYear() +
            '-' + pad(date.getMonth() + 1) +
            '-' + pad(date.getDate()) +
            'T' + pad(date.getHours()) +
            ':' + pad(date.getMinutes()) +
            ':' + pad(date.getSeconds());
    }
