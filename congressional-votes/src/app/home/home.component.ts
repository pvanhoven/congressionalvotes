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
  isLoading = true;
  senators: Senator[] = [];
  sessions: SenateSession[] = [];
  selectedSession = 0;

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.isLoading = true;
    combineLatest([
      timer(400), // minimum time (close to animation duration) before allowing binding to occur in below http request
      this.httpClient.get<SenatorsHomeResult>('senate/senators-home'),
    ]).subscribe(
      ([, data]) => {
        this.isLoading = false;
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
      },
      () => {
        this.isLoading = false;
      }
    );
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
