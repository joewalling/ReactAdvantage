import React from 'react';
import Enzyme, { shallow } from 'enzyme';

import Row from './index';

it('renders correctly', () => {
    const component = shallow(<Row />);

    expect(component).toMatchSnapshot();
});