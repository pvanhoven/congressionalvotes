import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscription } from 'rxjs';
import { Senator } from '../senator';
import { LegislativeItem } from '../legislative-item';
import { Vote } from '../vote';
import { ActivatedRoute } from '@angular/router';
import { switchMap, filter, tap } from 'rxjs/operators';

@Component({
  selector: 'app-senator-detail',
  templateUrl: './senator-detail.component.html',
  styleUrls: ['./senator-detail.component.css'],
})
export class SenatorDetailComponent implements OnInit, OnDestroy {
  CurrentSessionVote$: Observable<SenatorDetailViewModel>;
  ViewModel: SenatorDetailViewModel;
  isLoading = true;
  subscriptions: Subscription[] = [];

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.ViewModel = new SenatorDetailViewModel();
    this.ViewModel.Senator = {} as Senator;
    this.ViewModel.Votes = [] as SenatorDetailVote[];

    this.subscriptions.push(
      this.route.queryParams
        .pipe(
          switchMap((params) => {
            return this.httpClient.get<SenatorDetailViewModel>(
              `senate/currentsessionvotes?LisMemberId=${params['LisMemberId']}`
            );
          })
        )
        .subscribe(
          (r) => {
            this.isLoading = false;
            this.ViewModel = r;
          },
          () => {
            this.isLoading = false;
          }
        )
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((s) => s.unsubscribe());
  }
}

class SenatorDetailViewModel {
  Senator: Senator;

  Votes: SenatorDetailVote[];
}

class SenatorDetailVote {
  LegislativeItem: LegislativeItem;

  Vote: Vote;
}
