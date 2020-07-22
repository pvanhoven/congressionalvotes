import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Senator } from '../senator';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  senators$: Observable<Senator[]>;
  isLoading = true;

  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.senators$ = this.httpClient
      .get<Senator[]>('senate/senators?congressNumber=116&sessionNumber=2')
      .pipe(
        tap((senators) => {
          this.isLoading = false;
          return senators.sort((a, b) => {
            if (a.State < b.State) {
              return -1;
            }

            if (a.State > b.State) {
              return 1;
            }

            return 0;
          });
        })
      );
  }
}
