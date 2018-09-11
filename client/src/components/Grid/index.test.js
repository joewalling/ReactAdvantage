import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import { GridWrapper } from './index';

it('renders correctly', () => {
    const component = shallow(<GridWrapper />);

    expect(component).toMatchSnapshot();
});
