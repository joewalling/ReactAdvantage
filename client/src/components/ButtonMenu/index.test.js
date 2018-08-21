import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import ButtonMenu from './index';

it('renders correctly', () => {
    const component = shallow(<ButtonMenu />);

    expect(component).toMatchSnapshot();
});
