import { Component, HostListener } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { slideInAnimation } from './animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  animations: [slideInAnimation],
})
export class AppComponent {
  title = 'congressional-votes';
  enableAnimation = true;
  xDown = null;
  yDown = null;

  prepareRoute(outlet: RouterOutlet) {
    return (
      this.enableAnimation &&
      outlet &&
      outlet.activatedRouteData &&
      outlet.activatedRouteData.animation
    );
  }

  // would prefer this be an rjxs stream
  @HostListener('document:touchstart', ['$event'])
  handleTouchStart(evt: TouchEvent) {
    if (!evt || !evt.touches || evt.touches.length <= 0) {
      return;
    }

    if (!navigator.userAgent.includes('iphone')) {
      return;
    }

    // detect swipe right on iphone only in order to disable animation when
    // swiping right to go 'back' in browser stack. iOS has it's own
    // animation when swiping to go back and this animation looking clunky
    const firstTouch = evt.touches[0];
    this.xDown = firstTouch.clientX;
    this.yDown = firstTouch.clientY;
  }

  @HostListener('document:touchmove', ['$event'])
  handleTouchMove(evt) {
    if (!this.xDown || !this.yDown) {
      return;
    }

    var xUp = evt.touches[0].clientX;
    var yUp = evt.touches[0].clientY;

    var xDiff = this.xDown - xUp;
    var yDiff = this.yDown - yUp;

    if (Math.abs(xDiff) > Math.abs(yDiff)) {
      /*most significant*/
      if (xDiff > 0) {
        /* left swipe */
      } else {
        this.enableAnimation = false;
      }
    } else {
      if (yDiff > 0) {
        /* up swipe */
      } else {
        /* down swipe */
      }
    }

    /* reset values */
    this.xDown = null;
    this.yDown = null;
  }
}
