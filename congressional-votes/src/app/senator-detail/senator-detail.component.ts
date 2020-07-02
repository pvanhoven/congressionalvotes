import { Component, OnInit, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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
export class SenatorDetailComponent implements OnInit {
  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {}

  CurrentSessionVote$: Observable<SenatorDetailViewModel>;
  ViewModel: SenatorDetailViewModel;

  ngOnInit(): void {
    this.ViewModel = new SenatorDetailViewModel();
    this.ViewModel.Senator = {} as Senator;
    this.ViewModel.Votes = [] as SenatorDetailVote[];

    console.log('inside sen detail');
    //this.CurrentSessionVote$ = this.route.queryParams.pipe(
    this.route.queryParams
      .pipe(
        tap((params) => {
          console.log('querymaps: ' + params);
          console.log('querymaps: ' + Object.keys(params));
          console.log('querymaps:sen:' + params['LisMemberId']);
        }),
        switchMap((params) => {
          return this.httpClient.get<SenatorDetailViewModel>(
            `senate/currentsessionvotes?LisMemberId=${params['LisMemberId']}`
          );
        })
      )
      .subscribe((r) => {
        this.ViewModel = r;
      });
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
