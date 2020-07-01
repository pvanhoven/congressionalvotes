import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Senator } from './senator';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'congressional-votes';

  senators$: Observable<Senator[]>;

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.senators$ = this.httpClient
      .get<Senator[]>('senate/senators?congressNumber=116&sessionNumber=2')
      .pipe(
        tap((senators) =>
          senators.sort((a, b) => {
            if (a.State < b.State) {
              return -1;
            }

            if (a.State > b.State) {
              return 1;
            }

            return 0;
          })
        )
      );
  }
}
