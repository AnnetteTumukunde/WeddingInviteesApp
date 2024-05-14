using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeddingInviteesApp.Models;

namespace WeddingInviteesApp.Controllers
{
    public class EventRegistrationsController : Controller
    {
        private WIRSEntities db = new WIRSEntities();

        // GET: EventRegistrations
        public ActionResult Index()
        {
            var eventRegistrations = db.EventRegistrations.Include(e => e.Attendee1).Include(e => e.EventDetail);
            return View(eventRegistrations.ToList());
        }

        // GET: EventRegistrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return HttpNotFound();
            }
            return View(eventRegistration);
        }

        // GET: EventRegistrations/Create
        public ActionResult Create()
        {
            ViewBag.attendee = new SelectList(db.Attendees, "uuid", "email");
            ViewBag.eventid = new SelectList(db.EventDetails, "uuid", "eventName");
            return View();
        }

        // POST: EventRegistrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "uuid,eventid,attendee")] EventRegistration eventRegistration)
        {
            if (ModelState.IsValid)
            {
                var eventDetails = db.EventDetails.Find(eventRegistration.eventid);

                if (eventDetails != null)
                {
                    var registrationsCount = db.EventRegistrations.Count(er => er.eventid == eventRegistration.eventid);

                    if (registrationsCount < eventDetails.Venue1.capacity)
                    {
                        db.EventRegistrations.Add(eventRegistration);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry, the event is already full. Please try another event.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Event details not found.");
                }
            }

            ViewBag.attendee = new SelectList(db.Attendees, "uuid", "email", eventRegistration.attendee);
            ViewBag.eventid = new SelectList(db.EventDetails, "uuid", "eventName", eventRegistration.eventid);
            return View(eventRegistration);
        }

        // GET: EventRegistrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return HttpNotFound();
            }
            ViewBag.attendee = new SelectList(db.Attendees, "uuid", "email", eventRegistration.attendee);
            ViewBag.eventid = new SelectList(db.EventDetails, "uuid", "eventName", eventRegistration.eventid);
            return View(eventRegistration);
        }

        // POST: EventRegistrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "uuid,eventid,attendee")] EventRegistration eventRegistration)
        {
            if (ModelState.IsValid)
            {
                var eventDetails = db.EventDetails.Find(eventRegistration.eventid);

                if (eventDetails != null)
                {
                    var registrationsCount = db.EventRegistrations.Count(er => er.eventid == eventRegistration.eventid);

                    if (registrationsCount < eventDetails.Venue1.capacity)
                    {
                        db.Entry(eventRegistration).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sorry, the event is already full. Please try another event.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Event details not found.");
                }
            }
            ViewBag.attendee = new SelectList(db.Attendees, "uuid", "email", eventRegistration.attendee);
            ViewBag.eventid = new SelectList(db.EventDetails, "uuid", "eventName", eventRegistration.eventid);
            return View(eventRegistration);
        }

        // GET: EventRegistrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            if (eventRegistration == null)
            {
                return HttpNotFound();
            }
            return View(eventRegistration);
        }

        // POST: EventRegistrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventRegistration eventRegistration = db.EventRegistrations.Find(id);
            db.EventRegistrations.Remove(eventRegistration);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
