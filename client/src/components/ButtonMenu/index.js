import React, { Component } from 'react';
import { Menu } from 'primereact/menu';
import { Button } from 'primereact/button';

export default class ButtonMenu extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { items } = this.props;

    return (
      <div>
        <Menu
          model={items}
          popup={true}
          ref={el => (this.menu = el)}
          appendTo={document.body}
        />
        <Button label="&hellip;" onClick={event => this.menu.toggle(event)} />
      </div>
    );
  }
}
