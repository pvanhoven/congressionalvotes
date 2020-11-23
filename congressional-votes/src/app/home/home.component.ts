import { Component, OnInit } from '@angular/core';
import { Senator } from '../senator';
import { HttpClient } from '@angular/common/http';
import { SenateSession } from '../senate-session';
import { combineLatest, timer } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  skeletons = new Array(50);
  senators: Senator[] = [];
  sessions: SenateSession[] = [];
  selectedSession = 0;

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    combineLatest([
      timer(500), // minimum time (close to animation duration) before allowing binding to occur in below http request
      this.httpClient.get<SenatorsHomeResult>('senate/senators-home'),
    ]).subscribe(([, data]) => {
      this.selectedSession = data.CurrentSessionId;
      this.sessions = data.AvailableSessions;
      this.senators = data.CurrentSessionSenators.sort((a, b) => {
        if (a.State < b.State) {
          return -1;
        }

        if (a.State > b.State) {
          return 1;
        }

        return 0;
      });
    });
  }

  onSessionChanged(obj: any): void {
    const selectedSessionId = obj.value;
    const selectedSession = this.sessions.find(
      (s) => s.Id === selectedSessionId
    );
    if (selectedSession === undefined) {
      return;
    }

    this.httpClient
      .get<Senator[]>(
        `senate/senators?congressNumber=${selectedSession.CongressNumber}&sessionNumber=${selectedSession.SessionNumber}`
      )
      .subscribe((s) => {
        this.senators = s;
      });
  }
}

export class SenatorsHomeResult {
  CurrentSessionSenators: Senator[];

  CurrentSessionId: number;

  AvailableSessions: SenateSession[];
}
