import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { LocalNotifications } from '@capacitor/local-notifications';
import { AgendaItem } from '../types';

@Injectable({
  providedIn: 'root'
})
export class TalkReminderService {
  constructor(private ngZone: NgZone, public router: Router) {
    this.configure();
  }

  // Navigate the user to the talk they wanted to be reminded about
  async configure() {
  }

  async scheduleReminder(agendaItem: AgendaItem) {
    // Set reminder 5 minutes before the talk begins
    const timeSlice = agendaItem.startTime.substring(0, 5);
    const reminderDate = new Date(`December 8, 2021 ${timeSlice}:00`);
    reminderDate.setMinutes(reminderDate.getMinutes() - 5);
  }
}
