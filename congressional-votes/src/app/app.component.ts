import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SenateSession } from './senate-session';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'congressional-votes';

  sessions$: Observable<SenateSession[]>;

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.sessions$ = this.httpClient.get<SenateSession[]>("senate/sessions");
  }
}
