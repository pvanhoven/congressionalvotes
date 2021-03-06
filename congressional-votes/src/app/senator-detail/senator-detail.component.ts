import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { combineLatest, Observable, Subscription, timer } from 'rxjs';
import { Senator } from '../senator';
import { LegislativeItem } from '../legislative-item';
import { Vote } from '../vote';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-senator-detail',
  templateUrl: './senator-detail.component.html',
  styleUrls: ['./senator-detail.component.css'],
})
export class SenatorDetailComponent implements OnInit, OnDestroy {
  skeletons = new Array(50);
  CurrentSessionVote$: Observable<SenatorDetailViewModel>;
  ViewModel: SenatorDetailViewModel;
  subscriptions: Subscription[] = [];

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.ViewModel = new SenatorDetailViewModel();
    this.ViewModel.Senator = {} as Senator;
    this.ViewModel.Votes = [] as SenatorDetailVote[];

    this.subscriptions.push(
      combineLatest([
        timer(500), // minimium time before allowing binding to occur. Similar time to animation duration between routes
        this.route.queryParams.pipe(
          switchMap((params) => {
            return this.httpClient.get<SenatorDetailViewModel>(
              `senate/senatorsessionvotes?LisMemberId=${params['LisMemberId']}&sessionId=${params['SessionId']}`
            );
          })
        ),
      ]).subscribe(([, votes]) => {
        this.ViewModel = votes;
      })
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
